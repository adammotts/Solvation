import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { Title, Loading, Error } from '../../primitive';
import { Choices, Cards } from '../../components';
import './Train.css';

export function Train() {
  const { sessionId } = useParams();

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [playerCards, setPlayerCards] = useState([]);
  const [dealerCards, setDealerCards] = useState([]);
  const [choice, setChoice] = useState(null);
  const [actions, setActions] = useState({
    Hit: null,
    Stand: null,
    Double: null,
    Split: null,
  });

  function onSelect(move) {
    if (choice === null) {
      setChoice(move);
    }
  }

  useEffect(() => {
    fetch(`${process.env.REACT_APP_API_BASE_URL}/session/${sessionId}`, {
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
  }, [sessionId]);

  if (error) {
    console.log(error);
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
          <Choices allMoves={actions} onSelect={onSelect} afterMove={() => {}} choice={choice} />
        </>
      )}
    </div>
  );
}
