import React from 'react';
import { Title } from '../../primitive';
import { Choices, Cards } from '../../components';
import './Train.css';

export function Train() {
  const allMoves = {
    Play: -0.00511734,
    Abstain: 0,
  };

  return (
    <div className="train-background">
      <Title text={'What Would You Like To Do?'} />
      <Cards cards={[{ rank: 'A', suit: '♠' }]} />
      <Choices allMoves={allMoves} type={'Start'} />
    </div>
  );
}
