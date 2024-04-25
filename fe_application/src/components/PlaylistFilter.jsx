import { useContext, useState, useEffect } from 'react';
import { Box } from "@mui/material";
import { TextField } from "@mui/material";
import { Button } from "@mui/material";
import { PlaylistsContext, API_URL, AuthContext } from '../context/Context';

export default function PlaylistFilter({ openCreate, setOpenCreate }) {

    const { setPlaylists } = useContext(PlaylistsContext);
    const { right } = useContext(AuthContext);
    const [filterValue, setFilterValue] = useState("");

    useEffect(() => {
        let headers = { "Content-Type": "application/json" };
        if (right) {
            headers["X-AUTH-USERID"] = right.userId;
            headers["X-AUTH-USERNAME"] = right.username;
            headers["X-AUTH-ISEMPLOYEE"] = right.isEmployee;
        }
        const fetchOptions = { headers }
        let url = API_URL + "/playlist";
        if (filterValue) url += "?playlistName=" + filterValue;

        fetch(url, fetchOptions)
            .then(response => {
                if (response.status == 200) {
                    return response.json();
                }
                else console.log(response)
            })
            .then(data => setPlaylists(data));
    }, [filterValue, right])

    return <Box sx={{ display: 'inline-flex', py: 3 }}>
        <TextField id="playlist-filter" label="Name" variant="outlined" value={filterValue} onChange={e => setFilterValue(e.target.value)} />
        {right ? <Button variant="contained" onClick={() => setOpenCreate(true)}>Create new</Button> : null}
    </Box>
}