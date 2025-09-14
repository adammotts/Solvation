import React, { useState } from 'react';
import { Title, Button } from '../../primitive';
import { useNavigate } from 'react-router-dom';
import apiService from '../../services/api';
import Spade from '../../assets/spade.png';
import './Home.css';

export function Home() {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);

  const handleTrainClick = () => {
    setLoading(true);
    apiService.createSession()
      .then((data) => {
        navigate(`/train/${data.id}`);
        setLoading(false);
      })
      .catch((error) => {
        console.error('Error:', error);
        setLoading(false);
      });
  };

  return (
    <div className="home-background">
      <Title text={'Solvation'} />
      <div className="home-icon-container">
        <img src={Spade} alt="spade" className="spade-icon" />
      </div>
      <div className={`train-button-container`}>
        <Button text={'Train'} onClick={handleTrainClick} loading={loading} />
      </div>
    </div>
  );
}
