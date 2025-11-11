import React from 'react';
import { NavLink } from 'react-router-dom';
import { FaHome, FaEdit, FaUser } from 'react-icons/fa';

const BottomNav = () => {
  return (
    <nav className="bottom-nav">
      <NavLink 
        to="/" 
        className={({ isActive }) => `bottom-nav-item ${isActive ? 'active' : ''}`}
      >
        <FaHome className="nav-icon" />
        <span>首页</span>
      </NavLink>
      
      <NavLink 
        to="/editor" 
        className={({ isActive }) => `bottom-nav-item ${isActive ? 'active' : ''}`}
      >
        <FaEdit className="nav-icon" />
        <span>发布</span>
      </NavLink>
      
      <NavLink 
        to="/profile" 
        className={({ isActive }) => `bottom-nav-item ${isActive ? 'active' : ''}`}
      >
        <FaUser className="nav-icon" />
        <span>我的</span>
      </NavLink>
    </nav>
  );
};

export default BottomNav;
