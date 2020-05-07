import React from 'react';
import { NavLink } from 'react-router-dom';

const Header = () => (
    <header>
        <span>Series DB Microservices</span>
        <ul>
            <li><NavLink exact to="/">Home</NavLink></li>
            <li><NavLink to="/series">Series</NavLink></li>
            <li><NavLink to="/ratings">Ratings</NavLink></li>
            <li><NavLink to="/about">About</NavLink></li>
        </ul>
    </header>
);

export default Header;