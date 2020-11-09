import React from 'react';
import ClientsContext from '../ClientsContext';
import { EpisodeRatingsClient, SeriesRatingsClient } from '../typings/RatingsClients';
import { ImageClient, SeriesClient } from '../typings/SeriesAndEpisodesClients';

export default class ContainerComponent extends React.Component {

    static contextType = ClientsContext;

    componentWillMount() {
    
        let accessToken = localStorage.getItem('access_token');

        if (!!accessToken) {
            let seriesClient: SeriesClient = this.context.seriesClient;
            let imageClient: ImageClient = this.context.imageClient;
            let seriesRatingClient: SeriesRatingsClient = this.context.seriesRatingClient;
            let episodeRatingClient: EpisodeRatingsClient = this.context.episodeRatingClient;
            seriesClient.token = accessToken;
            imageClient.token = accessToken;
            seriesRatingClient.token = accessToken;
            episodeRatingClient.token = accessToken;
        }
      }

    render() {
        return (
            <div className="container">
                {this.props.children}
            </div>
        );
    }
}