import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { Title, Button, Loading, Error } from '../../primitive';
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
  const [terminal, setTerminal] = useState(false);

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
        setPlayerCards(data.hand.playerCards);
        setDealerCards(data.hand.dealerCards);
        setActions(data.actions);
        setTerminal(data.terminal);
        setLoading(false);
      })
      .catch((error) => {
        setLoading(false);
        setError(error);
      });
  }, [sessionId]);

  function afterMove(move) {
    setLoading(true);
    fetch(`${process.env.REACT_APP_API_BASE_URL}/session/${sessionId}`, {
      method: 'PATCH',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ move: move }),
    })
      .then((response) => response.json())
      .then((data) => {
        setPlayerCards(data.hand.playerCards);
        setDealerCards(data.hand.dealerCards);
        setActions(data.actions);
        setTerminal(data.terminal);
        setChoice(null);
        setLoading(false);
      })
      .catch((error) => {
        setError(error);
      });
  }

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
          {terminal ? (
            <Button text={'Next Hand'} onClick={() => window.location.reload()} />
          ) : (
            <Choices allMoves={actions} onSelect={onSelect} afterMove={afterMove} choice={choice} />
          )}
        </>
      )}
    </div>
  );
}
