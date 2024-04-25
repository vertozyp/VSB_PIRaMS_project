import { useContext, useState } from 'react';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import { Check, Info, Delete } from '@mui/icons-material';
import { PlaylistsContext, AuthContext } from '../context/Context';
import PlaylistDetailDialog from '../components/PlaylistDetailDialog';
import PlaylistDeleteDialog from '../components/PlaylistDeleteDialog';
import PlaylistCreateDialog from '../components/PlaylistCreateDialog';
import PlaylistFilter from '../components/PlaylistFilter';

export default function Body() {

    const { playlists } = useContext(PlaylistsContext);
    const { right } = useContext(AuthContext);
    let colNames = ["Id", "Name", "Public", "Detail", "Delete"]

    const [openDetail, setOpenDetail] = useState(false);
    const [detailId, setDetailId] = useState(-1);
    const [openDelete, setOpenDelete] = useState(false);
    const [idToDelete, setIdToDelete] = useState(-1);
    const [openCreate, setOpenCreate] = useState(false);

    const RowTemplate = ({ item }) => {
        return <TableRow>
            <TableCell>{item.playlistId}</TableCell>
            <TableCell>{item.name}</TableCell>
            <TableCell>{item.customerId ? null : <Check />}</TableCell>
            <TableCell><Info onClick={(event) => {
                setDetailId(parseInt(event.currentTarget.parentElement.parentElement.children[0].getInnerHTML()));
                setOpenDetail(true);
            }} sx={{ cursor: 'pointer' }} /></TableCell>
            <TableCell>{
                right != null && (
                    (item.customerId == null && right.isEmployee) ||
                    (item.customerId === right.userId)) ?
                    <Delete onClick={(event) => {
                        setIdToDelete(parseInt(event.currentTarget.parentElement.parentElement.children[0].getInnerHTML()));
                        setOpenDelete(true);
                    }} sx={{ cursor: 'pointer' }} /> : null
            }</TableCell>
        </TableRow>
    }

    return <>
        <PlaylistFilter openCreate={openCreate} setOpenCreate={setOpenCreate} />
        <TableContainer component={Paper}>
            <Table sx={{ minWidth: 650 }} aria-label="simple table" stickyHeader>
                <TableHead>
                    <TableRow>
                        {colNames.map(col => <TableCell key={col}>{col}</TableCell>)}
                    </TableRow>
                </TableHead>
                <TableBody>{
                    playlists ?
                        playlists.map(row => <RowTemplate key={row.playlistId} item={row} />) :
                        null}
                </TableBody>
            </Table>
        </TableContainer>
        <PlaylistDetailDialog
            selectedValue={detailId}
            open={openDetail}
            onClose={(value) => {
                setOpenDetail(false);
                setDetailId(value);
            }}
        />
        <PlaylistDeleteDialog
            selectedValue={idToDelete}
            open={openDelete}
            onClose={(value) => {
                setOpenDelete(false);
                setIdToDelete(value);
            }}
        />
        <PlaylistCreateDialog
            open={openCreate}
            onClose={() => { setOpenCreate(false); }}
        />
    </>
}