import React from "react";
import { ISeriesInfo } from "../typings/SeriesAndEpisodesClients";
import { Link, useRouteMatch } from "react-router-dom";

const Series = (props: { data: ISeriesInfo }) => {
  const match = useRouteMatch();
  return (
    <div>
      {props.data.id}:{" "}
      <Link to={`${match.path}/${props.data.id}`}>{props.data.title}</Link>:{" "}
      {props.data.description}
    </div>
  );
};

export default Series;
