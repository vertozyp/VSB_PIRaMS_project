import Stack from '@mui/material/Stack';
import { QueueMusic } from '@mui/icons-material';
import Login from '../components/Login';

export default function Header() {

    return <Stack direction="row" className="justify-content-around" sx={{ py: 3, boxShadow: 1 }}>
        <QueueMusic sx={{ fontSize: "4rem" }} />
        <h1>
            Create your own playlist!
        </h1>
        <Login />
    </Stack>
}