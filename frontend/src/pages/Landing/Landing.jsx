import React, { useState } from 'react';
import { Title } from '../../primitive';
import { Choices } from '../../components';
import { useNavigate } from 'react-router-dom';
import './Landing.css';

export function Landing() {
  const navigate = useNavigate();

  const [choice, setChoice] = useState(null);

  function onSelect(move) {
    if (choice === null) {
      setChoice(move);
    }
  }

  const allMoves = {
    Play: -0.00511734,
    Abstain: 0,
  };

  return (
    <div className="landing-background">
      <Title text={'Would you like to play some Blackjack?'} />
      <Choices allMoves={allMoves} onSelect={onSelect} afterMove={() => navigate('/welcome')} choice={choice} />
    </div>
  );
}
