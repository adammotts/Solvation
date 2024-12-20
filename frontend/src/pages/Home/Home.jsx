import React, { useState } from 'react';
import { Title, Button } from '../../primitive';
import { useNavigate } from 'react-router-dom';
import Spade from '../../assets/spade.png';
import './Home.css';

export function Home() {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);

  const handleTrainClick = () => {
    setLoading(true);
    fetch(`${process.env.REACT_APP_API_BASE_URL}/session`, {
      method: 'POST',
    })
      .then((response) => {
        if (response.ok) {
          return response.json();
        } else {
          throw new Error('Error generating session');
        }
      })
      .then((data) => {
        navigate(`/train/${data.id}`);
        setLoading(false);
      })
      .catch((error) => {
        console.error('Error:', error);
      });
  };

  return (
    <div className="home-background">
      <Title text={'Solvation'} />
      <img src={Spade} alt="spade" height={'250px'} />
      <div className={`train-button-container`}>
        <Button text={'Train'} onClick={handleTrainClick} loading={loading} />
      </div>
    </div>
  );
}
