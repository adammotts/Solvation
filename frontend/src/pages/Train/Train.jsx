import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Title, Subtitle, Button, Loading, Error } from '../../primitive';
import { Choices, Cards } from '../../components';
import './Train.css';

export function Train() {
  const { sessionId } = useParams();
  const navigate = useNavigate();

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
  const [ended, setEnded] = useState(false);
  const [evLoss, setEvLoss] = useState(0.0);
  const [statistics, setStatistics] = useState({
    bestMoves: 0,
    inaccuracies: 0,
    mistakes: 0,
    blunders: 0,
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
        if (!data.ended) {
          setPlayerCards(data.hand.playerCards);
          setDealerCards(data.hand.dealerCards);
          setActions(data.actions);
          setTerminal(data.terminal);
        }
        else {
          setEnded(true);
          setEvLoss(data.evLoss);
          setStatistics(data.statistics);
        }
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
      body: JSON.stringify({ move: move.name, label: move.label }),
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
          {!ended ? (
            <>
              <Title text={'What Would You Like To Do?'} />
              <Cards cards={dealerCards} variant={'dealer'} />
              <Cards cards={playerCards} variant={'player'} />
              {terminal ? (
                <Button text={'Next'} onClick={() => window.location.reload()} />
              ) : (
                <Choices allMoves={actions} onSelect={onSelect} afterMove={afterMove} choice={choice} />
              )}
            </>
          ) : (
            <>
              <Title text={'Session Ended'} />
              <Subtitle text={`Total EV Leaked: ${evLoss.toFixed(4)}`} />
              <Subtitle text={`Best Moves: ${statistics.bestMoves}`} />
              <Subtitle text={`Inaccuracies: ${statistics.inaccuracies}`} />
              <Subtitle text={`Mistakes: ${statistics.mistakes}`} />
              <Subtitle text={`Blunders: ${statistics.blunders}`} />
              <div className="return-home-button-container">
                <Button text={'Return Home'} onClick={() => navigate('/home')} />
              </div>
            </>
          )}
        </>
      )}
    </div>
  );
}
