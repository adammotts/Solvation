import React, { useState, useEffect } from 'react';
import { Loading, Error, Title } from '../../primitive';
import { Choices } from '../../components';
import { useNavigate } from 'react-router-dom';
import './Landing.css';

export function Landing() {
  const navigate = useNavigate();

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [gameExpectedValue, setGameExpectedValue] = useState(0);
  const [choice, setChoice] = useState(null);

  function onSelect(move) {
    if (choice === null) {
      setChoice(move);
    }
  }
    useEffect(() => {
      fetch(`${process.env.REACT_APP_API_BASE_URL}/game-expected-value`, {
        method: 'GET',
      })
        .then((response) => response.json())
        .then((data) => {
          setGameExpectedValue({
            Play: data.play,
            Abstain: data.abstain,
          });
          setLoading(false);
        })
        .catch((error) => {
          setLoading(false);
          setError(error);
        });
    }, []);
  
  if (error) {
    console.log(error);
    return <Error />;
  }

  return (
    <div className="landing-background">
      {loading ? (
        <Loading />
      ) : (
        <>
          <Title text={'Would you like to play some Blackjack?'} />
          <Choices allMoves={gameExpectedValue} onSelect={onSelect} afterMove={(move) => navigate('/welcome')} choice={choice} />
        </>
      )}
    </div>
  );
}
