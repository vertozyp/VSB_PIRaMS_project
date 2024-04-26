import Button from '@mui/material/Button';
import { Box } from '@mui/material';
import { Typography } from '@mui/material';
import { useContext } from 'react';
import { AuthContext } from '../context/Context';

export default function UserInfo() {

    const { right, setRight } = useContext(AuthContext);

    function handleClick() {
        setRight(null);
    }

    return <Box sx={{ flex: 1 }}>
        <Typography>Hello {right.username}!</Typography>
        <Button variant="contained" color="primary" onClick={handleClick}>
            Log out
        </Button>
    </Box>
}