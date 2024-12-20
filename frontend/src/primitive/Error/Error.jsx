import React from 'react';
import ErrorIcon from '../../assets/error.jpg';
import { Text } from '..';
import './Error.css';

export function Error() {
  return (
    <div className="error-background">
      <img src={ErrorIcon} alt="Error" height={'400px'} />
      <Text text={'An error occurred. Please try again later.'} />
    </div>
  );
}
