import Stack from '@mui/material/Stack';
import { QueueMusic } from '@mui/icons-material';
import Login from '../components/Login';

export default function Header() {

    return <Stack direction="row" className="justify-content-around">
        <QueueMusic />
        <div>
            Create your own playlist!
        </div>
        <Login />
    </Stack>
}