import Button from '@mui/material/Button';
import { FormControl } from '@mui/base/FormControl';
import TextField from '@mui/material/TextField';
import { useContext, useState } from 'react';
import { AuthContext, API_URL } from '../context/Context';

export default function LoginForm() {

    const { setRight } = useContext(AuthContext);
    const [inputValue, setInputValue] = useState("");

    function handleSubmit() {

        const fetchOptions = {
            method: "POST",
            body: JSON.stringify({ username: inputValue }),
            headers: {
                "Content-Type": "application/json"
            }
        }

        fetch(API_URL + "/login", fetchOptions)
            .then(response => {
                if (response.status == 200) {
                    return response.json();
                }
                else console.log(response)
            })
            .then(data => setRight(data));
    }

    return <FormControl sx={{ p: 2 }}>
        <TextField id="login-form" label="Username" variant="outlined" value={inputValue} onChange={e => setInputValue(e.target.value)} />
        <Button variant="contained" color="primary" type='submit' onClick={handleSubmit}>
            Log in
        </Button>
    </FormControl>
}