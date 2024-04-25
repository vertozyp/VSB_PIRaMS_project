import { useContext } from 'react';
import { AuthContext } from '../context/Context';
import LoginForm from './LoginForm';
import UserInfo from './UserInfo';

export default function Login() {

    const { right } = useContext(AuthContext);

    return (
        right ?
            <UserInfo /> :
            <LoginForm />
    )
}