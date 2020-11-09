import React from "react";
import { RouteComponentProps } from "react-router-dom";
import ClientsContext from '../ClientsContext';
import { EpisodeRatingsClient, SeriesRatingsClient } from "../typings/RatingsClients";
import { ImageClient, SeriesClient } from '../typings/SeriesAndEpisodesClients';

interface IProps extends RouteComponentProps {

}

interface IState {
    accessToken: string | null;
}

export default class SignInComponent extends React.Component<IProps, IState> {

    constructor(props: IProps) {
        super(props);
        this.state = {
            accessToken: ""
        };
      }
    
    static contextType = ClientsContext;

    componentDidMount() {
    
        let seriesClient: SeriesClient = this.context.seriesClient;
        let imageClient: ImageClient = this.context.imageClient;
        let seriesRatingClient: SeriesRatingsClient = this.context.seriesRatingClient;
        let episodeRatingClient: EpisodeRatingsClient = this.context.episodeRatingClient;

        let query = new URLSearchParams(this.props.location.search);
        let accessToken = query.get("access_token");

        if (!!accessToken) {
            seriesClient.token = accessToken;
            imageClient.token = accessToken;
            seriesRatingClient.token = accessToken;
            episodeRatingClient.token = accessToken;
            localStorage.setItem("access_token", accessToken);
        } else {
            accessToken = localStorage.getItem('access_token');
        }
        this.setState(() => ({accessToken: accessToken}));
      }

    render() {
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
            <a href={`/Account/Authenticate?returnUrl=${this.props.location.pathname}`}>Sign in</a>
            <p>
                access_token: {this.state.accessToken ? this.state.accessToken : ''}
            </p>
            </div>
      );
    }

}