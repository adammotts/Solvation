import React, { useState, useEffect } from 'react';
import { Title, Subtitle, Button } from '../../primitive';
import { useNavigate } from 'react-router-dom';
import './Welcome.css';

export function Welcome() {
  const [isVisible, setIsVisible] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const timer = setTimeout(() => {
      setIsVisible(true);
    }, 5000);
    return () => clearTimeout(timer);
  }, []);

  return (
    <div className="welcome-background">
      <Title text={'Welcome!'} />
      <div className="subtitle-container">
        <Subtitle
          text={
            'Whether or not you chose to be here, you are here now, so hear me out.'
          }
        />
        <Subtitle
          text={
            'Many people choose to gamble, even if they are aware that it is a losing endeavor.'
          }
        />
        <Subtitle
          text={
            'Especially in Blackjack, many people are unaware of optimal strategy, losing even more money than they should.'
          }
        />
      </div>
      <div className={`next-button-container ${isVisible ? 'visible' : ''}`}>
        <Button text={'Next'} onClick={() => navigate('/home')} />
      </div>
    </div>
  );
}
