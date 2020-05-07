import React from 'react';
import { ISeriesRatingInfo } from '../typings/RatingsClients';

const Rating = (props: { data: ISeriesRatingInfo }) => {

    return (<div>
        {props.data.id}: {props.data.opinion}: {props.data.rating}: {props.data.seriesId}: {props.data.userId}
        </div>);
}

export default Rating;