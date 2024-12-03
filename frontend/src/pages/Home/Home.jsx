import React from 'react';
import { Subtitle, Title } from '../../primitive';
import { ModalButton } from '../../components';
import './Home.css';

export function Home() {
  return (
    <div className="home-background">
      <Title text={'Welcome to Solvation!'} />
      <Subtitle text={'Would you like to play some Blackjack?'} />
      <ModalButton
        buttonText={'Yes, I love gambling!'}
        playedMove={'Play'}
        allMoves={{
          Play: -0.7,
          Abstain: 0,
        }}
      />
    </div>
  );
}
