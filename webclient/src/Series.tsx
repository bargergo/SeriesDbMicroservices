import React from 'react';
import { SeriesInfo } from './typings/SeriesAndEpisodesClients';

function Series(props: { data: SeriesInfo }) {

    return (<div>
        {props.data.id}: {props.data.title}
        </div>);
}

export default Series;