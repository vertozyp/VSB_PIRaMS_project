import { createContext } from 'react';

export const AuthContext = createContext({right: null, setRight: ()=>{}});
export const PlaylistsContext = createContext({playlists: null, setPlaylists: ()=>{}});
export const TracksContext = createContext({tracks: null, setTracks: ()=>{}});

export const API_URL = "http://localhost:5107";