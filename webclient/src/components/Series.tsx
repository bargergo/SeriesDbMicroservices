import React from "react";
import { ISeriesInfo } from "../typings/SeriesAndEpisodesClients";
import { Link, useRouteMatch } from "react-router-dom";
import "./Series.css";

const Series = (props: { data: ISeriesInfo }) => {
  const match = useRouteMatch();
  return (
    <>
      <img
        className="cover-small"
        src={`/api/Images/${props.data.imageId}`}
        alt="cover"
      ></img>
      <Link to={`${match.path}/${props.data.id}`}>
        {props.data.title} ({props.data.firstAired.getFullYear()})
      </Link>
      {props.data.description}
    </>
  );
};

export default Series;
