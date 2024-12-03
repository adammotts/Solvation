import React from 'react';
import { Subtitle, Title } from '../../primitive';
import { ModalButton } from '../../components';

export function Home() {
  return (
    <div style={styles.background}>
      <Title text={'Welcome to Solvation!'} />
      <Subtitle text={'Would you like to play some Blackjack?'} />
      <ModalButton buttonText={'Yes, I love gambling!'} modalContent={'test'} />
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
