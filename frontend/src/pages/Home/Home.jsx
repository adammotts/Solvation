import React from 'react';
import { Title, Button } from '../../primitive';
import { useNavigate } from 'react-router-dom';
import Spade from '../../assets/spade.png';
import './Home.css';

export function Home() {
  const navigate = useNavigate();

  return (
    <div className="home-background">
      <Title text={'Solvation'} />
      <img className="spade" src={Spade} alt="spade" height={'250px'} />
      <div className={`train-button-container`}>
        <Button text={'Train'} onClick={() => navigate('/home')} />
      </div>
    </div>
  );
}
