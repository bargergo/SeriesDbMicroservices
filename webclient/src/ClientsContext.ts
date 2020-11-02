import React from "react";
import { EpisodeRatingsClient, SeriesRatingsClient } from "./typings/RatingsClients";
import { ImageClient, SeriesClient } from "./typings/SeriesAndEpisodesClients";

const ClientsContext = React.createContext({
    seriesClient: new SeriesClient(),
    imageClient: new ImageClient(),
    seriesRatingClient: new SeriesRatingsClient(),
    episodeRatingClient: new EpisodeRatingsClient()
});

export default ClientsContext;