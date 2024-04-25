import * as React from 'react';
import { useContext, useState, useEffect } from 'react';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import { DialogContent } from '@mui/material';
import { DialogActions } from '@mui/material';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Paper from '@mui/material/Paper';
import Button from '@mui/material/Button';
import { AuthContext, TracksContext, API_URL } from '../context/Context';

export default function PlaylistCreateDialog({ onClose, selectedValue, open }) {

    const { tracks } = useContext(TracksContext);
    const { right } = useContext(AuthContext);
    const [newName, setNewName] = useState("");
    const [trackFilter, setTrackFilter] = useState("");
    const [composerFilter, setComposerFilter] = useState("");
    const [newTracks, setNewTracks] = useState([]);
    const [filteredTracks, setFilteredTracks] = useState([]);

    useEffect(() => {
        setFilteredTracks(tracks);
    }, [tracks]);

    useEffect(() => {
        if (tracks) {
            let newlyFilteredTracks;
            if (!trackFilter && !composerFilter) newlyFilteredTracks = tracks;
            if (trackFilter && !composerFilter) newlyFilteredTracks = tracks.filter(x => x.name.includes(trackFilter));
            if (!trackFilter && composerFilter) newlyFilteredTracks = tracks.filter(x => x.composer && x.composer.includes(composerFilter));
            if (trackFilter && composerFilter) newlyFilteredTracks = tracks.filter(x => x.name.includes(trackFilter) && x.composer && x.composer.includes(composerFilter));

            setFilteredTracks([...newlyFilteredTracks]);
        }
    }, [trackFilter, composerFilter]);

    function addTrack(event) {
        const trackId = parseInt(event.target.parentElement.childNodes[0].getInnerHTML());
        const track = filteredTracks.find((x) => x.trackId === trackId);
        setNewTracks([...newTracks, track]);
    }

    function removeTrack(event) {
        const trackId = parseInt(event.target.parentElement.childNodes[0].getInnerHTML());
        const tracks = newTracks.filter((x) => x.trackId != trackId);
        setNewTracks([...tracks]);
    }

    function confirmCreation() {
        if (right) {
            let headers = {
                "Content-Type": "application/json",
                "X-AUTH-USERID": right.userId,
                "X-AUTH-USERNAME": right.username,
                "X-AUTH-ISEMPLOYEE": right.isEmployee,
            };
            const data = {
                name: newName,
                trackIds: newTracks.map(x => x.trackId)
            }
            const fetchOptions = {
                headers,
                method: "POST",
                body: JSON.stringify(data)
            }

            fetch(API_URL + "/playlist", fetchOptions)
                .then(response => {
                    if (response.status == 201) {
                        console.log("CREATED")
                        console.log(response)
                        onClose();
                    }
                    else {
                        console.log("FAILED")
                        console.log(response)
                    }
                })
        }
    }

    const RowTemplate = ({ item }) => {
        return <TableRow onClick={(event) => addTrack(event)} sx={{ cursor: 'pointer' }}>
            <TableCell>{item.trackId}</TableCell>
            <TableCell>{item.name}</TableCell>
            <TableCell>{item.composer}</TableCell>
        </TableRow>
    }

    return (
        <Dialog maxWidth="lg" onClose={() => { onClose(selectedValue) }} open={open}>
            <DialogContent>
                <DialogTitle>Create new playlist</DialogTitle>
                <TextField id="playlist-create-name" label="Name" variant="outlined" value={newName} onChange={e => setNewName(e.target.value)} />
                <DialogTitle>Tracks</DialogTitle>
                <Box sx={{ display: 'inline-flex' }}>
                    <TextField id="playlist-create-track" label="Track" variant="outlined" value={trackFilter} onChange={e => setTrackFilter(e.target.value)} />
                    <TextField id="playlist-detail-composer" label="Composer" variant="outlined" value={composerFilter} onChange={e => setComposerFilter(e.target.value)} />
                </Box>
                <Box sx={{ display: 'inline-flex' }}>
                    <TableContainer component={Paper}>
                        <Table sx={{ minWidth: 650 }} aria-label="simple table" stickyHeader>
                            <TableHead>
                                <TableRow>
                                    <TableCell key={"id"}>{"Id"}</TableCell>
                                    <TableCell key={"name"}>{"Name"}</TableCell>
                                    <TableCell key={"composer"}>{"Composer"}</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {filteredTracks ?
                                    filteredTracks.map(row => <RowTemplate key={row.trackId} item={row} />) :
                                    null}
                            </TableBody>
                        </Table>
                    </TableContainer>
                    <TableContainer component={Paper}>
                        <Table sx={{ minWidth: 650 }} aria-label="simple table" stickyHeader>
                            <TableHead>
                                <TableRow>
                                    <TableCell key={"id"}>{"Id"}</TableCell>
                                    <TableCell key={"name"}>{"Name"}</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {newTracks ?
                                    newTracks.map(row => {
                                        return <TableRow onClick={(event) => removeTrack(event)} sx={{ cursor: 'pointer' }}>
                                            <TableCell>{row.trackId}</TableCell>
                                            <TableCell>{row.name}</TableCell>
                                        </TableRow>
                                    }) :
                                    null}
                            </TableBody>
                        </Table>
                    </TableContainer>
                </Box>
            </DialogContent>
            <DialogActions>
                <Button variant="contained" color="success" onClick={confirmCreation} >Create</Button>
            </DialogActions>
        </Dialog>
    );
}