import Button from '@mui/material/Button';
import { useContext } from 'react';
import { AuthContext } from '../context/Context';

export default function UserInfo() {

    const { right, setRight } = useContext(AuthContext);

    function handleClick() {
        setRight(null);
    }

    return <>
        <div>Hello {right.username}!</div>
        <Button variant="contained" color="primary" onClick={handleClick}>
            Log out
        </Button>
    </>
}