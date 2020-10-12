import { render } from "@testing-library/react";
import React from "react";
import { MemoryRouter, Route } from "react-router-dom";
import Series from "../../components/Series";
import { ISeriesInfo } from "../../typings/SeriesAndEpisodesClients";

it("should render the title", () => {
    const series: ISeriesInfo = {
        id: "seriesId1",
        title: "American Chopper",
        description: "Best show ever!",
        firstAired: new Date(2018, 11, 24),
        lastUpdated: new Date(Date.now()),
        imageId: ""
    };
    const { getByText } = render(
        <MemoryRouter>
            <Route path="/" component={() => <Series data={series}/>} />
        </MemoryRouter>
    );
    expect(
        getByText(/American Chopper/i)
      ).toBeInTheDocument();
});

it("should render the year when it first aired", () => {
    const series: ISeriesInfo = {
        id: "seriesId1",
        title: "American Chopper",
        description: "Best show ever!",
        firstAired: new Date(2018, 11, 24),
        lastUpdated: new Date(Date.now()),
        imageId: ""
    };
    const { getByText } = render(
        <MemoryRouter>
            <Route path="/" component={() => <Series data={series}/>} />
        </MemoryRouter>
    );
    expect(
        getByText(/2018/i)
      ).toBeInTheDocument();
});