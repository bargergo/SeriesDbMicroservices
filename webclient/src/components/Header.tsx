import React from "react";
import { Container, Nav, Navbar, NavItem } from "react-bootstrap";
import { Link, NavLink } from "react-router-dom";

const Header = () => (
  <header>
    <Navbar bg="primary" variant="dark" expand="lg">
      <Container>
        <Navbar.Brand as={Link} to="/">
          Series DB Microservices
        </Navbar.Brand>
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="mr-auto">
            <NavItem>
              <Nav.Link as={NavLink} exact to="/">
                Home
              </Nav.Link>
            </NavItem>
            <NavItem>
              <Nav.Link as={NavLink} to="/series">
                Series
              </Nav.Link>
            </NavItem>
            <NavItem>
              <Nav.Link as={NavLink} to="/ratings">
                Ratings
              </Nav.Link>
            </NavItem>
            <NavItem>
              <Nav.Link as={NavLink} to="/about">
                About
              </Nav.Link>
            </NavItem>
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  </header>
);

export default Header;
