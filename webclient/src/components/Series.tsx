import React from 'react';
import { ISeriesInfo } from '../typings/SeriesAndEpisodesClients';

const Series = (props: { data: ISeriesInfo }) => {

    return (<div>
        {props.data.id}: {props.data.title}: {props.data.description}
        </div>);
}

export default Series;