import './App.css';
import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';

import { useState, useEffect } from 'react';
import Stack from '@mui/material/Stack';
import Header from './layout/Header';
import Body from './layout/Body';
import Footer from './layout/Footer';
import { AuthContext, PlaylistsContext, TracksContext, API_URL } from './context/Context';

function App() {

  const [right, setRight] = useState(null);
  const [playlists, setPlaylists] = useState(null);
  const [tracks, setTracks] = useState(null);

  useEffect(() => {
    const fetchOptions = {
      headers: {
        "Content-Type": "application/json"
      }
    }

    fetch(API_URL + "/playlist", fetchOptions)
      .then(response => {
        if (response.status == 200) {
          return response.json();
        }
        else console.log(response)
      })
      .then(data => setPlaylists(data));

    fetch(API_URL + "/track", fetchOptions)
      .then(response => {
        if (response.status == 200) {
          return response.json();
        }
        else console.log(response)
      })
      .then(data => setTracks(data));
  }, []);

  return (
    <AuthContext.Provider value={{ right, setRight }}>
      <PlaylistsContext.Provider value={{ playlists, setPlaylists }}>
        <TracksContext.Provider value={{ tracks, setTracks }}>
          <Stack className="vh-100 d-flex flex-column ">
            <Header />
            <Body />
            <Footer />
          </Stack>
        </TracksContext.Provider>
      </PlaylistsContext.Provider>
    </AuthContext.Provider>
  );
}

export default App;
