import axios from 'axios';
import React from "react";

const SignInComponent = (props: any) => {
    let query = new URLSearchParams(props.location.search);
    let accessToken = query.get("access_token");
    if (!!accessToken) {
        axios.defaults.headers.common['Authorization'] = "Bearer " + accessToken;
    }
    return (
        <div className="main-content">
        <h2>About</h2>
        <p>
            The front end course directory lists many of the courses we teach on
            HTML, CSS, JavaScript and more! Be sure to visit the Teachers section to
            view a list of our talented teachers. Or visit the Courses section and
            select a topic -- HTML, CSS, or JavaScript -- to see a list of our
            courses.
        </p>
        <a href={`/Account/Authenticate?returnUrl=${props.location.pathname}`}>Sign in</a>
        <p>
            access_token: {accessToken}
        </p>
        </div>
  );
};

export default SignInComponent;
