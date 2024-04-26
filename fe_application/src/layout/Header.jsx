import Stack from '@mui/material/Stack';
import { QueueMusic } from '@mui/icons-material';
import { Typography } from '@mui/material';
import Login from '../components/Login';

export default function Header() {

    return <Stack direction="row" className="justify-content-around" sx={{ py: 3, boxShadow: 1 }}>
        <QueueMusic sx={{ fontSize: "4rem", flex: 1 }} />
        <Typography variant="h3" component="h3" sx={{ flex: 2, textAlign: "center" }}>
            Create your own playlist!
        </Typography>
        <Login />
    </Stack>
}