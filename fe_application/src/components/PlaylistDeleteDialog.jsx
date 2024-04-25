import { useContext } from "react";
import { Dialog } from "@mui/material";
import { DialogTitle } from "@mui/material";
import { Button } from "@mui/material";
import { API_URL, AuthContext } from '../context/Context';

export default function PlaylistDeleteDialog({ onClose, selectedValue, open }) {

    const { right } = useContext(AuthContext);

    function onclick() {
        if (right) {
            let headers = {
                "Content-Type": "application/json",
                "X-AUTH-USERID": right.userId,
                "X-AUTH-USERNAME": right.username,
                "X-AUTH-ISEMPLOYEE": right.isEmployee,
            };

            const fetchOptions = {
                headers,
                method: "DELETE"
            }
            let url = API_URL + "/playlist/" + selectedValue;

            fetch(url, fetchOptions)
                .then(response => {
                    if (response.status == 204) {
                        console.log("SUCCESS")
                        console.log(response)
                        onClose(selectedValue);
                    }
                    else {
                        console.log("FAIL")
                        console.log(response)
                    }
                })
        }
    }

    return <Dialog onClose={() => { onClose(selectedValue) }} open={open}>
        <DialogTitle>Do you really wish to delete this playlist?</DialogTitle>
        <Button variant="contained" color="error" onClick={onclick}>Delete</Button>
    </Dialog>
}