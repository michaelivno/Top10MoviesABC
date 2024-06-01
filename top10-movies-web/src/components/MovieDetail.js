import React from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button, Box, Typography } from '@mui/material';

function MovieDetail({ movie, onClose }) {
  return (
    <Dialog open={Boolean(movie)} onClose={onClose}>
      <DialogContent>
        <Box display="flex" flexDirection="column" alignItems="center" position="relative">
          {movie.imageUrl && (
            <Box position="relative" display="flex" justifyContent="center" alignItems="center">
              <img src={movie.imageUrl} alt={movie.title} style={{ maxWidth: '100%', display: 'block' }} />
              <Typography
                variant="h4"
                style={{
                  position: 'absolute',
                  color: 'white',
                  backgroundColor: 'rgba(0, 0, 0, 0.5)',
                  padding: '8px 16px',
                  borderRadius: '4px',
                  textAlign: 'center',
                }}
              >
                {movie.title}
              </Typography>
            </Box>
          )}
          <Typography variant="body1" style={{ marginTop: '16px' }}><strong>Category:</strong> {movie.category}</Typography>
          <Typography variant="body1"><strong>Ranking:</strong> {movie.ranking}</Typography>
        </Box>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose} color="primary">Close</Button>
      </DialogActions>
    </Dialog>
  );
}

export default MovieDetail;
