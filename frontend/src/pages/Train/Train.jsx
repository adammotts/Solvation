import React, { useState } from 'react';
import { Title } from '../../primitive';
import { Choices, Cards } from '../../components';
import './Train.css';

export function Train() {
  const [playerCards, setPlayerCards] = useState([]);
  const [dealerCards, setDealerCards] = useState([]);
  const [actions, setActions] = useState({
    Hit: null,
    Stand: null,
    Double: null,
    Split: null,
  });

  fetch('http://localhost:5256/hand/6765085c864009ec961ea2e8', {
    method: 'GET',
  })
    .then((response) => response.json())
    .then((data) => {
      setPlayerCards(data.hand.playerCards);
      setDealerCards(data.hand.dealerCards);
      setActions(data.actions);
    })
    .catch((error) => {
      console.error('Error:', error);
    });

  return (
    <div className="train-background">
      <Title text={'What Would You Like To Do?'} />
      <Cards cards={dealerCards} variant={'dealer'} />
      <Cards cards={playerCards} variant={'player'} />
      <Choices allMoves={actions} type={'Start'} />
    </div>
  );
}
