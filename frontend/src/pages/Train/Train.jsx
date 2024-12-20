import React, { useState, useEffect } from 'react';
import { Title, Loading, Error } from '../../primitive';
import { Choices, Cards } from '../../components';
import './Train.css';

export function Train() {
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [playerCards, setPlayerCards] = useState([]);
  const [dealerCards, setDealerCards] = useState([]);
  const [actions, setActions] = useState({
    Hit: null,
    Stand: null,
    Double: null,
    Split: null,
  });

  useEffect(() => {
    fetch('http://localhost:5256/hand/676520f3de03c062e7185970', {
      method: 'GET',
    })
      .then((response) => response.json())
      .then((data) => {
        setLoading(false);
        setPlayerCards(data.hand.playerCards);
        setDealerCards(data.hand.dealerCards);
        setActions(data.actions);
      })
      .catch((error) => {
        setLoading(false);
        setError(error);
      });
  }, []);

  if (error) {
    return <Error />;
  }

  return (
    <div className="train-background">
      {loading ? (
        <Loading />
      ) : (
        <>
          <Title text={'What Would You Like To Do?'} />
          <Cards cards={dealerCards} variant={'dealer'} />
          <Cards cards={playerCards} variant={'player'} />
          <Choices allMoves={actions} type={'Start'} />
        </>
      )}
    </div>
  );
}
