import React from 'react';
import { Title } from '../../primitive';
import { Choices } from '../../components';
import './Landing.css';

export function Landing() {
  const allMoves = {
    Play: -0.00511734,
    Abstain: 0,
  };

  return (
    <div className="landing-background">
      <Title text={'Would you like to play some Blackjack?'} />
      <Choices allMoves={allMoves} type={'Start'} />
    </div>
  );
}
