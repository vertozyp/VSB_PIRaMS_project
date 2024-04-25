import * as React from 'react';
import { useContext, useState, useEffect } from 'react';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Paper from '@mui/material/Paper';
import { PlaylistsContext, TracksContext } from '../context/Context';

export default function PlaylistDetailDialog({ onClose, selectedValue, open }) {

    const { playlists } = useContext(PlaylistsContext);
    const { tracks } = useContext(TracksContext);
    const [actualId, setActualId] = useState();
    const [actualName, setActualName] = useState();
    const [actualPuclic, setActualPublic] = useState();
    const [actualtracks, setActualTracks] = useState();

    function setActualDetail(detail) {
        setActualId(detail.playlistId);
        setActualName(detail.name);
        setActualPublic(detail.customerId ? false : true);
        setActualTracks(detail.tracks);
    }

    useEffect(() => {
        if (playlists) {
            let detail = playlists.find((playlist) => playlist.playlistId === selectedValue);
            let trackList = [];
            if (detail) {
                for (const trackId of detail.trackIds) {
                    trackList.push(tracks.find((track) => track.trackId === trackId));
                }
                detail.tracks = trackList;
                setActualDetail(detail);
            }
        }
    }, [selectedValue])

    const RowTemplate = ({ item }) => {
        return <TableRow>
            <TableCell>{item.trackId}</TableCell>
            <TableCell>{item.name}</TableCell>
            <TableCell>{item.composer}</TableCell>
        </TableRow>
    }

    return (
        <Dialog maxWidth="lg" onClose={() => { onClose(selectedValue) }} open={open}>
            <DialogTitle>Playlist detail</DialogTitle>
            <Box sx={{ display: 'inline-flex' }}>
                <TextField id="playlist-detail-playlistId" label="Id" variant="outlined" InputProps={{ readOnly: true, }} value={actualId} />
                <TextField id="playlist-detail-name" label="Name" variant="outlined" InputProps={{ readOnly: true, }} value={actualName} />
                <TextField id="playlist-detail-public" label="Public" variant="outlined" InputProps={{ readOnly: true, }} value={actualPuclic} />
            </Box>
            <DialogTitle>Tracks</DialogTitle>
            <TableContainer component={Paper}>
                <Table sx={{ minWidth: 650 }} aria-label="simple table" stickyHeader>
                    <TableHead>
                        <TableRow>
                            <TableCell key={"id"}>{"Id"}</TableCell>
                            <TableCell key={"name"}>{"Name"}</TableCell>
                            <TableCell key={"composer"}>{"Composer"}</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>{
                        actualtracks ?
                            actualtracks.map(row => <RowTemplate key={row.trackId} item={row} />) :
                            null}
                    </TableBody>
                </Table>
            </TableContainer>
        </Dialog>
    );
}