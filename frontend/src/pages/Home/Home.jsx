import React from 'react';
import { Button, Subtitle, Title } from '../../components';

export function Home() {
  return (
    <div style={styles.background}>
      <Title text={'Welcome to Solvation!'} />
      <Subtitle text={'Would you like to play some Blackjack?'} />
      <Button text={'Yes, I love gambling!'} onClick={undefined} />
    </div>
  );
}

const styles = {
  background: {
    backgroundColor: 'darkgreen',
    height: '100vh',
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    flexDirection: 'column',
    gap: '20px',
  },
};
