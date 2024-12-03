import React from 'react';
import { Title } from '../../primitive';
import { Choices } from '../../components';
import './Home.css';

export function Home() {
  const allMoves = {
    Play: -0.00511734,
    Abstain: 0,
  };

  return (
    <div className="home-background">
      <Title text={'Would you like to play some Blackjack?'} />
      <Choices allMoves={allMoves} type={'Start'} />
    </div>
  );
}
