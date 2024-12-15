import React from 'react';
import { Title, Subtitle, Button } from '../../primitive';
import { useNavigate } from 'react-router-dom';
import './Home.css';

export function Home() {
  const navigate = useNavigate();

  return (
    <div className="home-background">
      <Title text={'Solvation'} />
      <Subtitle text={'Train your Blackjack skills'} />
      <div className={`train-button-container`}>
        <Button text={'Train'} onClick={() => navigate('/home')} />
      </div>
    </div>
  );
}
