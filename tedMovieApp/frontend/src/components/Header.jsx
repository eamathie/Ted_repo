
import React from "react";
import "./Header.css";
import team_logo from "../assets/team_logo.png";
import Navbar from "./Navbar";

export default function Header() {
    return (
        <header>
            <div className="header-container">
                <img
                    src={team_logo}
                    id="header-logo"
                    alt="App Logo" />
            </div>
        </header>
    );
}
