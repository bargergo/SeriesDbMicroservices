import { render } from "@testing-library/react";
import React from "react";
import { MemoryRouter, Route } from "react-router-dom";
import ClientsContext from "../../ClientsContext";
import SeriesDetailComponent from "../../components/SeriesDetailComponent";
import {
  EpisodeRatingsClient,
  SeriesRatingsClient,
} from "../../typings/RatingsClients";
import {
  CreateEpisodeRequest,
  EpisodeDetail,
  FileParameter,
  ImageClient,
  SeasonDetail,
  SeriesAdminClient,
  SeriesClient,
  SeriesDetail,
  SeriesInfo,
  UpsertSeriesRequest,
} from "../../typings/SeriesAndEpisodesClients";

it("should render the average rating", async () => {
  const fakeSeriesDetail: SeriesDetail = new SeriesDetail({
    seasons: [
      new SeasonDetail({
        id: 1,
        episodes: [
          new EpisodeDetail({
            id: 1,
            title: "Pilot",
            description: "string",
            firstAired: new Date(2010, 10, 11),
            lastUpdated: new Date(2010, 10, 18),
          }),
        ],
      }),
    ],
    averageRating: 5.6,
    numberOfRatings: 24,
    id: "seriesId234",
    title: "American Chopper",
    description: "Best show ever",
    firstAired: new Date(2010, 10, 11),
    lastUpdated: new Date(Date.now()),
    imageId: "imageId",
  });
  class FakeSeriesClient extends SeriesClient {
    getAllSeries(): Promise<SeriesInfo[]> {
      throw new Error("Method not implemented.");
    }
    createSeries(body: UpsertSeriesRequest | undefined): Promise<SeriesDetail> {
      throw new Error("Method not implemented.");
    }
    getSeries(id: string | null): Promise<SeriesDetail> {
      return new Promise<SeriesDetail>((resolveFunction, _errorFunction) =>
        resolveFunction(fakeSeriesDetail)
      );
    }
    updateSeries(
      id: string | null,
      body: UpsertSeriesRequest | undefined
    ): Promise<void> {
      throw new Error("Method not implemented.");
    }
    deleteSeries(id: string | null): Promise<void> {
      throw new Error("Method not implemented.");
    }
    uploadImage(
      id: string | null,
      image: FileParameter | null | undefined
    ): Promise<void> {
      throw new Error("Method not implemented.");
    }
    addEpisode(
      id: string | null,
      seasonId: number,
      body: CreateEpisodeRequest | undefined
    ): Promise<void> {
      throw new Error("Method not implemented.");
    }
    deleteEpisode(
      id: string | null,
      seasonId: number,
      episodeId: number
    ): Promise<void> {
      throw new Error("Method not implemented.");
    }
    updateEpisode(
      id: string | null,
      seasonId: number,
      episodeId: number,
      body: CreateEpisodeRequest | undefined
    ): Promise<void> {
      throw new Error("Method not implemented.");
    }
    getEpisode(
      id: string | null,
      seasonId: number,
      episodeId: number
    ): Promise<EpisodeDetail> {
      throw new Error("Method not implemented.");
    }
  }

  const { getByText } = render(
    <ClientsContext.Provider
      value={{
        seriesClient: new FakeSeriesClient(),
        imageClient: new ImageClient(),
        seriesRatingClient: new SeriesRatingsClient(),
        episodeRatingClient: new EpisodeRatingsClient(),
        seriesAdminClient: new SeriesAdminClient()
      }}
    >
      <MemoryRouter initialEntries={["/seriesId234"]}>
        <Route
          path="/:id"
          render={(props) => <SeriesDetailComponent {...props} />}
        />
      </MemoryRouter>
    </ClientsContext.Provider>
  );
  await flushPromises();
  expect(getByText(/5.6\s\/\s10/i)).toBeInTheDocument();
});

const flushPromises = () => new Promise(setImmediate);
