import React from 'react';
import PropTypes from 'prop-types';
import './Cards.css';

import TwoOfClubs from '../../assets/cards/2_of_clubs.png';
import TwoOfDiamonds from '../../assets/cards/2_of_diamonds.png';
import TwoOfHearts from '../../assets/cards/2_of_hearts.png';
import TwoOfSpades from '../../assets/cards/2_of_spades.png';
import ThreeOfClubs from '../../assets/cards/3_of_clubs.png';
import ThreeOfDiamonds from '../../assets/cards/3_of_diamonds.png';
import ThreeOfHearts from '../../assets/cards/3_of_hearts.png';
import ThreeOfSpades from '../../assets/cards/3_of_spades.png';
import FourOfClubs from '../../assets/cards/4_of_clubs.png';
import FourOfDiamonds from '../../assets/cards/4_of_diamonds.png';
import FourOfHearts from '../../assets/cards/4_of_hearts.png';
import FourOfSpades from '../../assets/cards/4_of_spades.png';
import FiveOfClubs from '../../assets/cards/5_of_clubs.png';
import FiveOfDiamonds from '../../assets/cards/5_of_diamonds.png';
import FiveOfHearts from '../../assets/cards/5_of_hearts.png';
import FiveOfSpades from '../../assets/cards/5_of_spades.png';
import SixOfClubs from '../../assets/cards/6_of_clubs.png';
import SixOfDiamonds from '../../assets/cards/6_of_diamonds.png';
import SixOfHearts from '../../assets/cards/6_of_hearts.png';
import SixOfSpades from '../../assets/cards/6_of_spades.png';
import SevenOfClubs from '../../assets/cards/7_of_clubs.png';
import SevenOfDiamonds from '../../assets/cards/7_of_diamonds.png';
import SevenOfHearts from '../../assets/cards/7_of_hearts.png';
import SevenOfSpades from '../../assets/cards/7_of_spades.png';
import EightOfClubs from '../../assets/cards/8_of_clubs.png';
import EightOfDiamonds from '../../assets/cards/8_of_diamonds.png';
import EightOfHearts from '../../assets/cards/8_of_hearts.png';
import EightOfSpades from '../../assets/cards/8_of_spades.png';
import NineOfClubs from '../../assets/cards/9_of_clubs.png';
import NineOfDiamonds from '../../assets/cards/9_of_diamonds.png';
import NineOfHearts from '../../assets/cards/9_of_hearts.png';
import NineOfSpades from '../../assets/cards/9_of_spades.png';
import TenOfClubs from '../../assets/cards/10_of_clubs.png';
import TenOfDiamonds from '../../assets/cards/10_of_diamonds.png';
import TenOfHearts from '../../assets/cards/10_of_hearts.png';
import TenOfSpades from '../../assets/cards/10_of_spades.png';
import JackOfClubs from '../../assets/cards/jack_of_clubs.png';
import JackOfDiamonds from '../../assets/cards/jack_of_diamonds.png';
import JackOfHearts from '../../assets/cards/jack_of_hearts.png';
import JackOfSpades from '../../assets/cards/jack_of_spades.png';
import QueenOfClubs from '../../assets/cards/queen_of_clubs.png';
import QueenOfDiamonds from '../../assets/cards/queen_of_diamonds.png';
import QueenOfHearts from '../../assets/cards/queen_of_hearts.png';
import QueenOfSpades from '../../assets/cards/queen_of_spades.png';
import KingOfClubs from '../../assets/cards/king_of_clubs.png';
import KingOfDiamonds from '../../assets/cards/king_of_diamonds.png';
import KingOfHearts from '../../assets/cards/king_of_hearts.png';
import KingOfSpades from '../../assets/cards/king_of_spades.png';
import AceOfClubs from '../../assets/cards/ace_of_clubs.png';
import AceOfDiamonds from '../../assets/cards/ace_of_diamonds.png';
import AceOfHearts from '../../assets/cards/ace_of_hearts.png';
import AceOfSpades from '../../assets/cards/ace_of_spades.png';

export function Cards({ cards }) {
  function getImage(card) {
    const cardMap = {
      '2♣': TwoOfClubs,
      '2♦': TwoOfDiamonds,
      '2♥': TwoOfHearts,
      '2♠': TwoOfSpades,
      '3♣': ThreeOfClubs,
      '3♦': ThreeOfDiamonds,
      '3♥': ThreeOfHearts,
      '3♠': ThreeOfSpades,
      '4♣': FourOfClubs,
      '4♦': FourOfDiamonds,
      '4♥': FourOfHearts,
      '4♠': FourOfSpades,
      '5♣': FiveOfClubs,
      '5♦': FiveOfDiamonds,
      '5♥': FiveOfHearts,
      '5♠': FiveOfSpades,
      '6♣': SixOfClubs,
      '6♦': SixOfDiamonds,
      '6♥': SixOfHearts,
      '6♠': SixOfSpades,
      '7♣': SevenOfClubs,
      '7♦': SevenOfDiamonds,
      '7♥': SevenOfHearts,
      '7♠': SevenOfSpades,
      '8♣': EightOfClubs,
      '8♦': EightOfDiamonds,
      '8♥': EightOfHearts,
      '8♠': EightOfSpades,
      '9♣': NineOfClubs,
      '9♦': NineOfDiamonds,
      '9♥': NineOfHearts,
      '9♠': NineOfSpades,
      '10♣': TenOfClubs,
      '10♦': TenOfDiamonds,
      '10♥': TenOfHearts,
      '10♠': TenOfSpades,
      'J♣': JackOfClubs,
      'J♦': JackOfDiamonds,
      'J♥': JackOfHearts,
      'J♠': JackOfSpades,
      'Q♣': QueenOfClubs,
      'Q♦': QueenOfDiamonds,
      'Q♥': QueenOfHearts,
      'Q♠': QueenOfSpades,
      'K♣': KingOfClubs,
      'K♦': KingOfDiamonds,
      'K♥': KingOfHearts,
      'K♠': KingOfSpades,
      'A♣': AceOfClubs,
      'A♦': AceOfDiamonds,
      'A♥': AceOfHearts,
      'A♠': AceOfSpades,
    };

    return cardMap[`${card.rank}${card.suit}`];
  }

  return (
    <div className="cards-background">
      {cards.map((card, index) => (
        <img key={index} src={getImage(card)} alt={card} height={'250px'} />
      ))}
    </div>
  );
}

Cards.propTypes = {
  cards: PropTypes.arrayOf(
    PropTypes.shape({
      rank: PropTypes.string.isRequired,
      suit: PropTypes.string.isRequired,
    })
  ).isRequired,
};
