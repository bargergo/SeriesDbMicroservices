﻿/* tslint:disable */
/* eslint-disable */
//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.4.2.0 (NJsonSchema v10.1.11.0 (Newtonsoft.Json v12.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------
// ReSharper disable InconsistentNaming

import axios, { AxiosInstance, AxiosRequestConfig, AxiosResponse } from 'axios';

export class SeriesRatingsClient {
    private instance: AxiosInstance;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(baseUrl?: string, instance?: AxiosInstance) {
        this.instance = instance ? instance : axios.create();
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    createSeriesRating(body: SeriesRatingData | undefined): Promise<Created201Response> {
        let url_ = this.baseUrl + "/api/SeriesRatings";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(body);

        let options_ = <AxiosRequestConfig>{
            validateStatus: () => true,
            data: content_,
            method: "POST",
            url: url_,
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        };

        return this.instance.request(options_).then((_response: AxiosResponse) => {
            return this.processCreateSeriesRating(_response);
        });
    }

    protected processCreateSeriesRating(response: AxiosResponse): Promise<Created201Response> {
        const status = response.status;
        let _headers: any = {};
        if (response.headers && typeof response.headers === "object") {
            for (let k in response.headers) {
                if (response.headers.hasOwnProperty(k)) {
                    _headers[k] = response.headers[k];
                }
            }
        }
        if (status === 201) {
            const _responseText = response.data;
            let result201: any = null;
            let resultData201  = _responseText;
            result201 = Created201Response.fromJS(resultData201);
            return result201;
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.data;
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Promise.resolve<Created201Response>(<any>null);
    }

    /**
     * Get Episodes Ratings Endpoint
     * @param userId (optional) id of the user
     * @param seriesId (optional) seriesId
     * @return OK
     */
    getSeriesRatings(userId: number | undefined, seriesId: string | undefined): Promise<SeriesRatingInfo[]> {
        let url_ = this.baseUrl + "/api/SeriesRatings?";
        if (userId === null)
            throw new Error("The parameter 'userId' cannot be null.");
        else if (userId !== undefined)
            url_ += "userId=" + encodeURIComponent("" + userId) + "&";
        if (seriesId === null)
            throw new Error("The parameter 'seriesId' cannot be null.");
        else if (seriesId !== undefined)
            url_ += "seriesId=" + encodeURIComponent("" + seriesId) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_ = <AxiosRequestConfig>{
            validateStatus: () => true,
            method: "GET",
            url: url_,
            headers: {
                "Accept": "application/json"
            }
        };

        return this.instance.request(options_).then((_response: AxiosResponse) => {
            return this.processGetSeriesRatings(_response);
        });
    }

    protected processGetSeriesRatings(response: AxiosResponse): Promise<SeriesRatingInfo[]> {
        const status = response.status;
        let _headers: any = {};
        if (response.headers && typeof response.headers === "object") {
            for (let k in response.headers) {
                if (response.headers.hasOwnProperty(k)) {
                    _headers[k] = response.headers[k];
                }
            }
        }
        if (status === 200) {
            const _responseText = response.data;
            let result200: any = null;
            let resultData200  = _responseText;
            if (Array.isArray(resultData200)) {
                result200 = [] as any;
                for (let item of resultData200)
                    result200!.push(SeriesRatingInfo.fromJS(item));
            }
            return result200;
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.data;
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Promise.resolve<SeriesRatingInfo[]>(<any>null);
    }

    /**
     * @param id id of the series rating
     * @return OK
     */
    getSeriesRating(id: number): Promise<SeriesRatingInfo> {
        let url_ = this.baseUrl + "/api/SeriesRatings/{id}";
        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url_ = url_.replace("{id}", encodeURIComponent("" + id));
        url_ = url_.replace(/[?&]$/, "");

        let options_ = <AxiosRequestConfig>{
            validateStatus: () => true,
            method: "GET",
            url: url_,
            headers: {
                "Accept": "application/json"
            }
        };

        return this.instance.request(options_).then((_response: AxiosResponse) => {
            return this.processGetSeriesRating(_response);
        });
    }

    protected processGetSeriesRating(response: AxiosResponse): Promise<SeriesRatingInfo> {
        const status = response.status;
        let _headers: any = {};
        if (response.headers && typeof response.headers === "object") {
            for (let k in response.headers) {
                if (response.headers.hasOwnProperty(k)) {
                    _headers[k] = response.headers[k];
                }
            }
        }
        if (status === 200) {
            const _responseText = response.data;
            let result200: any = null;
            let resultData200  = _responseText;
            result200 = SeriesRatingInfo.fromJS(resultData200);
            return result200;
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.data;
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Promise.resolve<SeriesRatingInfo>(<any>null);
    }

    /**
     * @param id id of the series rating
     */
    deleteSeriesRating(id: number): Promise<NoContent204Response> {
        let url_ = this.baseUrl + "/api/SeriesRatings/{id}";
        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url_ = url_.replace("{id}", encodeURIComponent("" + id));
        url_ = url_.replace(/[?&]$/, "");

        let options_ = <AxiosRequestConfig>{
            validateStatus: () => true,
            method: "DELETE",
            url: url_,
            headers: {
                "Accept": "application/json"
            }
        };

        return this.instance.request(options_).then((_response: AxiosResponse) => {
            return this.processDeleteSeriesRating(_response);
        });
    }

    protected processDeleteSeriesRating(response: AxiosResponse): Promise<NoContent204Response> {
        const status = response.status;
        let _headers: any = {};
        if (response.headers && typeof response.headers === "object") {
            for (let k in response.headers) {
                if (response.headers.hasOwnProperty(k)) {
                    _headers[k] = response.headers[k];
                }
            }
        }
        if (status === 204) {
            const _responseText = response.data;
            let result204: any = null;
            let resultData204  = _responseText;
            result204 = NoContent204Response.fromJS(resultData204);
            return result204;
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.data;
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Promise.resolve<NoContent204Response>(<any>null);
    }

    /**
     * @param id id of the series rating
     * @param body (optional) 
     */
    updateSeriesRating(id: number, body: SeriesRatingData | undefined): Promise<NoContent204Response> {
        let url_ = this.baseUrl + "/api/SeriesRatings/{id}";
        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url_ = url_.replace("{id}", encodeURIComponent("" + id));
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(body);

        let options_ = <AxiosRequestConfig>{
            validateStatus: () => true,
            data: content_,
            method: "PUT",
            url: url_,
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        };

        return this.instance.request(options_).then((_response: AxiosResponse) => {
            return this.processUpdateSeriesRating(_response);
        });
    }

    protected processUpdateSeriesRating(response: AxiosResponse): Promise<NoContent204Response> {
        const status = response.status;
        let _headers: any = {};
        if (response.headers && typeof response.headers === "object") {
            for (let k in response.headers) {
                if (response.headers.hasOwnProperty(k)) {
                    _headers[k] = response.headers[k];
                }
            }
        }
        if (status === 204) {
            const _responseText = response.data;
            let result204: any = null;
            let resultData204  = _responseText;
            result204 = NoContent204Response.fromJS(resultData204);
            return result204;
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.data;
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Promise.resolve<NoContent204Response>(<any>null);
    }

    /**
     * @param seriesId id of the series
     * @return OK
     */
    getAverageRatingForSeries(seriesId: string): Promise<AverageOfRatingsResponse> {
        let url_ = this.baseUrl + "/api/SeriesRatings/Series/{seriesId}/Average";
        if (seriesId === undefined || seriesId === null)
            throw new Error("The parameter 'seriesId' must be defined.");
        url_ = url_.replace("{seriesId}", encodeURIComponent("" + seriesId));
        url_ = url_.replace(/[?&]$/, "");

        let options_ = <AxiosRequestConfig>{
            validateStatus: () => true,
            method: "GET",
            url: url_,
            headers: {
                "Accept": "application/json"
            }
        };

        return this.instance.request(options_).then((_response: AxiosResponse) => {
            return this.processGetAverageRatingForSeries(_response);
        });
    }

    protected processGetAverageRatingForSeries(response: AxiosResponse): Promise<AverageOfRatingsResponse> {
        const status = response.status;
        let _headers: any = {};
        if (response.headers && typeof response.headers === "object") {
            for (let k in response.headers) {
                if (response.headers.hasOwnProperty(k)) {
                    _headers[k] = response.headers[k];
                }
            }
        }
        if (status === 200) {
            const _responseText = response.data;
            let result200: any = null;
            let resultData200  = _responseText;
            result200 = AverageOfRatingsResponse.fromJS(resultData200);
            return result200;
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.data;
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Promise.resolve<AverageOfRatingsResponse>(<any>null);
    }
}

export class EpisodeRatingsClient {
    private instance: AxiosInstance;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(baseUrl?: string, instance?: AxiosInstance) {
        this.instance = instance ? instance : axios.create();
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    createEpisodeRating(body: EpisodeRatingData | undefined): Promise<Created201Response> {
        let url_ = this.baseUrl + "/api/EpisodeRatings";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(body);

        let options_ = <AxiosRequestConfig>{
            validateStatus: () => true,
            data: content_,
            method: "POST",
            url: url_,
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        };

        return this.instance.request(options_).then((_response: AxiosResponse) => {
            return this.processCreateEpisodeRating(_response);
        });
    }

    protected processCreateEpisodeRating(response: AxiosResponse): Promise<Created201Response> {
        const status = response.status;
        let _headers: any = {};
        if (response.headers && typeof response.headers === "object") {
            for (let k in response.headers) {
                if (response.headers.hasOwnProperty(k)) {
                    _headers[k] = response.headers[k];
                }
            }
        }
        if (status === 201) {
            const _responseText = response.data;
            let result201: any = null;
            let resultData201  = _responseText;
            result201 = Created201Response.fromJS(resultData201);
            return result201;
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.data;
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Promise.resolve<Created201Response>(<any>null);
    }

    /**
     * Get Episodes Ratings Endpoint
     * @param userId (optional) id of the user
     * @param seriesId (optional) seriesId
     * @param seasonId (optional) seasonId
     * @param episodeId (optional) episodeId
     * @return OK
     */
    getEpisodeRatings(userId: number | undefined, seriesId: string | undefined, seasonId: number | undefined, episodeId: number | undefined): Promise<EpisodeRatingInfo[]> {
        let url_ = this.baseUrl + "/api/EpisodeRatings?";
        if (userId === null)
            throw new Error("The parameter 'userId' cannot be null.");
        else if (userId !== undefined)
            url_ += "userId=" + encodeURIComponent("" + userId) + "&";
        if (seriesId === null)
            throw new Error("The parameter 'seriesId' cannot be null.");
        else if (seriesId !== undefined)
            url_ += "seriesId=" + encodeURIComponent("" + seriesId) + "&";
        if (seasonId === null)
            throw new Error("The parameter 'seasonId' cannot be null.");
        else if (seasonId !== undefined)
            url_ += "seasonId=" + encodeURIComponent("" + seasonId) + "&";
        if (episodeId === null)
            throw new Error("The parameter 'episodeId' cannot be null.");
        else if (episodeId !== undefined)
            url_ += "episodeId=" + encodeURIComponent("" + episodeId) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_ = <AxiosRequestConfig>{
            validateStatus: () => true,
            method: "GET",
            url: url_,
            headers: {
                "Accept": "application/json"
            }
        };

        return this.instance.request(options_).then((_response: AxiosResponse) => {
            return this.processGetEpisodeRatings(_response);
        });
    }

    protected processGetEpisodeRatings(response: AxiosResponse): Promise<EpisodeRatingInfo[]> {
        const status = response.status;
        let _headers: any = {};
        if (response.headers && typeof response.headers === "object") {
            for (let k in response.headers) {
                if (response.headers.hasOwnProperty(k)) {
                    _headers[k] = response.headers[k];
                }
            }
        }
        if (status === 200) {
            const _responseText = response.data;
            let result200: any = null;
            let resultData200  = _responseText;
            if (Array.isArray(resultData200)) {
                result200 = [] as any;
                for (let item of resultData200)
                    result200!.push(EpisodeRatingInfo.fromJS(item));
            }
            return result200;
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.data;
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Promise.resolve<EpisodeRatingInfo[]>(<any>null);
    }

    /**
     * @param id id of the episode rating
     */
    getEpisodeRating(id: number): Promise<EpisodeRatingInfo> {
        let url_ = this.baseUrl + "/api/EpisodeRatings/{id}";
        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url_ = url_.replace("{id}", encodeURIComponent("" + id));
        url_ = url_.replace(/[?&]$/, "");

        let options_ = <AxiosRequestConfig>{
            validateStatus: () => true,
            method: "GET",
            url: url_,
            headers: {
                "Accept": "application/json"
            }
        };

        return this.instance.request(options_).then((_response: AxiosResponse) => {
            return this.processGetEpisodeRating(_response);
        });
    }

    protected processGetEpisodeRating(response: AxiosResponse): Promise<EpisodeRatingInfo> {
        const status = response.status;
        let _headers: any = {};
        if (response.headers && typeof response.headers === "object") {
            for (let k in response.headers) {
                if (response.headers.hasOwnProperty(k)) {
                    _headers[k] = response.headers[k];
                }
            }
        }
        if (status === 200) {
            const _responseText = response.data;
            let result200: any = null;
            let resultData200  = _responseText;
            result200 = EpisodeRatingInfo.fromJS(resultData200);
            return result200;
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.data;
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Promise.resolve<EpisodeRatingInfo>(<any>null);
    }

    /**
     * @param id id of the episode rating
     */
    deleteEpisodeRating(id: number): Promise<NoContent204Response> {
        let url_ = this.baseUrl + "/api/EpisodeRatings/{id}";
        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url_ = url_.replace("{id}", encodeURIComponent("" + id));
        url_ = url_.replace(/[?&]$/, "");

        let options_ = <AxiosRequestConfig>{
            validateStatus: () => true,
            method: "DELETE",
            url: url_,
            headers: {
                "Accept": "application/json"
            }
        };

        return this.instance.request(options_).then((_response: AxiosResponse) => {
            return this.processDeleteEpisodeRating(_response);
        });
    }

    protected processDeleteEpisodeRating(response: AxiosResponse): Promise<NoContent204Response> {
        const status = response.status;
        let _headers: any = {};
        if (response.headers && typeof response.headers === "object") {
            for (let k in response.headers) {
                if (response.headers.hasOwnProperty(k)) {
                    _headers[k] = response.headers[k];
                }
            }
        }
        if (status === 204) {
            const _responseText = response.data;
            let result204: any = null;
            let resultData204  = _responseText;
            result204 = NoContent204Response.fromJS(resultData204);
            return result204;
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.data;
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Promise.resolve<NoContent204Response>(<any>null);
    }

    /**
     * @param id id of the episode rating
     * @param body (optional) 
     */
    updateEpisodeRating(id: number, body: EpisodeRatingData | undefined): Promise<NoContent204Response> {
        let url_ = this.baseUrl + "/api/EpisodeRatings/{id}";
        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url_ = url_.replace("{id}", encodeURIComponent("" + id));
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(body);

        let options_ = <AxiosRequestConfig>{
            validateStatus: () => true,
            data: content_,
            method: "PUT",
            url: url_,
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        };

        return this.instance.request(options_).then((_response: AxiosResponse) => {
            return this.processUpdateEpisodeRating(_response);
        });
    }

    protected processUpdateEpisodeRating(response: AxiosResponse): Promise<NoContent204Response> {
        const status = response.status;
        let _headers: any = {};
        if (response.headers && typeof response.headers === "object") {
            for (let k in response.headers) {
                if (response.headers.hasOwnProperty(k)) {
                    _headers[k] = response.headers[k];
                }
            }
        }
        if (status === 204) {
            const _responseText = response.data;
            let result204: any = null;
            let resultData204  = _responseText;
            result204 = NoContent204Response.fromJS(resultData204);
            return result204;
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.data;
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Promise.resolve<NoContent204Response>(<any>null);
    }

    /**
     * @param seriesId id of the series
     * @return OK
     */
    getAverageRatingForSeries2(seriesId: string): Promise<AverageOfRatingsResponse> {
        let url_ = this.baseUrl + "/api/EpisodeRatings/Series/{seriesId}/Average";
        if (seriesId === undefined || seriesId === null)
            throw new Error("The parameter 'seriesId' must be defined.");
        url_ = url_.replace("{seriesId}", encodeURIComponent("" + seriesId));
        url_ = url_.replace(/[?&]$/, "");

        let options_ = <AxiosRequestConfig>{
            validateStatus: () => true,
            method: "GET",
            url: url_,
            headers: {
                "Accept": "application/json"
            }
        };

        return this.instance.request(options_).then((_response: AxiosResponse) => {
            return this.processGetAverageRatingForSeries2(_response);
        });
    }

    protected processGetAverageRatingForSeries2(response: AxiosResponse): Promise<AverageOfRatingsResponse> {
        const status = response.status;
        let _headers: any = {};
        if (response.headers && typeof response.headers === "object") {
            for (let k in response.headers) {
                if (response.headers.hasOwnProperty(k)) {
                    _headers[k] = response.headers[k];
                }
            }
        }
        if (status === 200) {
            const _responseText = response.data;
            let result200: any = null;
            let resultData200  = _responseText;
            result200 = AverageOfRatingsResponse.fromJS(resultData200);
            return result200;
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.data;
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Promise.resolve<AverageOfRatingsResponse>(<any>null);
    }

    /**
     * @param seriesId id of the series
     * @param seasonId id of the season
     * @return OK
     */
    getAverageRatingForSeason(seriesId: string, seasonId: number): Promise<AverageOfRatingsResponse> {
        let url_ = this.baseUrl + "/api/EpisodeRatings/Series/{seriesId}/Season/{seasonId}/Average";
        if (seriesId === undefined || seriesId === null)
            throw new Error("The parameter 'seriesId' must be defined.");
        url_ = url_.replace("{seriesId}", encodeURIComponent("" + seriesId));
        if (seasonId === undefined || seasonId === null)
            throw new Error("The parameter 'seasonId' must be defined.");
        url_ = url_.replace("{seasonId}", encodeURIComponent("" + seasonId));
        url_ = url_.replace(/[?&]$/, "");

        let options_ = <AxiosRequestConfig>{
            validateStatus: () => true,
            method: "GET",
            url: url_,
            headers: {
                "Accept": "application/json"
            }
        };

        return this.instance.request(options_).then((_response: AxiosResponse) => {
            return this.processGetAverageRatingForSeason(_response);
        });
    }

    protected processGetAverageRatingForSeason(response: AxiosResponse): Promise<AverageOfRatingsResponse> {
        const status = response.status;
        let _headers: any = {};
        if (response.headers && typeof response.headers === "object") {
            for (let k in response.headers) {
                if (response.headers.hasOwnProperty(k)) {
                    _headers[k] = response.headers[k];
                }
            }
        }
        if (status === 200) {
            const _responseText = response.data;
            let result200: any = null;
            let resultData200  = _responseText;
            result200 = AverageOfRatingsResponse.fromJS(resultData200);
            return result200;
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.data;
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Promise.resolve<AverageOfRatingsResponse>(<any>null);
    }

    /**
     * @param seriesId id of the series
     * @param seasonId id of the season
     * @param episodeId id of the episode
     * @return OK
     */
    getAverageRatingForEpisode(seriesId: string, seasonId: number, episodeId: number): Promise<AverageOfRatingsResponse> {
        let url_ = this.baseUrl + "/api/EpisodeRatings/Series/{seriesId}/Season/{seasonId}/Episode/{episodeId}/Average";
        if (seriesId === undefined || seriesId === null)
            throw new Error("The parameter 'seriesId' must be defined.");
        url_ = url_.replace("{seriesId}", encodeURIComponent("" + seriesId));
        if (seasonId === undefined || seasonId === null)
            throw new Error("The parameter 'seasonId' must be defined.");
        url_ = url_.replace("{seasonId}", encodeURIComponent("" + seasonId));
        if (episodeId === undefined || episodeId === null)
            throw new Error("The parameter 'episodeId' must be defined.");
        url_ = url_.replace("{episodeId}", encodeURIComponent("" + episodeId));
        url_ = url_.replace(/[?&]$/, "");

        let options_ = <AxiosRequestConfig>{
            validateStatus: () => true,
            method: "GET",
            url: url_,
            headers: {
                "Accept": "application/json"
            }
        };

        return this.instance.request(options_).then((_response: AxiosResponse) => {
            return this.processGetAverageRatingForEpisode(_response);
        });
    }

    protected processGetAverageRatingForEpisode(response: AxiosResponse): Promise<AverageOfRatingsResponse> {
        const status = response.status;
        let _headers: any = {};
        if (response.headers && typeof response.headers === "object") {
            for (let k in response.headers) {
                if (response.headers.hasOwnProperty(k)) {
                    _headers[k] = response.headers[k];
                }
            }
        }
        if (status === 200) {
            const _responseText = response.data;
            let result200: any = null;
            let resultData200  = _responseText;
            result200 = AverageOfRatingsResponse.fromJS(resultData200);
            return result200;
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.data;
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Promise.resolve<AverageOfRatingsResponse>(<any>null);
    }
}

export class AverageOfRatingsResponse implements IAverageOfRatingsResponse {
    average!: number;
    count!: number;

    constructor(data?: IAverageOfRatingsResponse) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.average = _data["average"];
            this.count = _data["count"];
        }
    }

    static fromJS(data: any): AverageOfRatingsResponse {
        data = typeof data === 'object' ? data : {};
        let result = new AverageOfRatingsResponse();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["average"] = this.average;
        data["count"] = this.count;
        return data; 
    }
}

export interface IAverageOfRatingsResponse {
    average: number;
    count: number;
}

export class Created201Response implements ICreated201Response {

    constructor(data?: ICreated201Response) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
    }

    static fromJS(data: any): Created201Response {
        data = typeof data === 'object' ? data : {};
        let result = new Created201Response();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        return data; 
    }
}

export interface ICreated201Response {
}

export class EpisodeRatingData implements IEpisodeRatingData {
    episodeId!: number;
    opinion!: string;
    rating!: number;
    seasonId!: number;
    seriesId!: string;
    userId!: number;

    constructor(data?: IEpisodeRatingData) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.episodeId = _data["episodeId"];
            this.opinion = _data["opinion"];
            this.rating = _data["rating"];
            this.seasonId = _data["seasonId"];
            this.seriesId = _data["seriesId"];
            this.userId = _data["userId"];
        }
    }

    static fromJS(data: any): EpisodeRatingData {
        data = typeof data === 'object' ? data : {};
        let result = new EpisodeRatingData();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["episodeId"] = this.episodeId;
        data["opinion"] = this.opinion;
        data["rating"] = this.rating;
        data["seasonId"] = this.seasonId;
        data["seriesId"] = this.seriesId;
        data["userId"] = this.userId;
        return data; 
    }
}

export interface IEpisodeRatingData {
    episodeId: number;
    opinion: string;
    rating: number;
    seasonId: number;
    seriesId: string;
    userId: number;
}

export class EpisodeRatingInfo implements IEpisodeRatingInfo {
    episodeId!: number;
    id!: number;
    opinion!: string;
    rating!: number;
    seasonId!: number;
    seriesId!: string;
    userId!: number;

    constructor(data?: IEpisodeRatingInfo) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.episodeId = _data["episodeId"];
            this.id = _data["id"];
            this.opinion = _data["opinion"];
            this.rating = _data["rating"];
            this.seasonId = _data["seasonId"];
            this.seriesId = _data["seriesId"];
            this.userId = _data["userId"];
        }
    }

    static fromJS(data: any): EpisodeRatingInfo {
        data = typeof data === 'object' ? data : {};
        let result = new EpisodeRatingInfo();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["episodeId"] = this.episodeId;
        data["id"] = this.id;
        data["opinion"] = this.opinion;
        data["rating"] = this.rating;
        data["seasonId"] = this.seasonId;
        data["seriesId"] = this.seriesId;
        data["userId"] = this.userId;
        return data; 
    }
}

export interface IEpisodeRatingInfo {
    episodeId: number;
    id: number;
    opinion: string;
    rating: number;
    seasonId: number;
    seriesId: string;
    userId: number;
}

export class NoContent204Response implements INoContent204Response {

    constructor(data?: INoContent204Response) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
    }

    static fromJS(data: any): NoContent204Response {
        data = typeof data === 'object' ? data : {};
        let result = new NoContent204Response();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        return data; 
    }
}

export interface INoContent204Response {
}

export class SeriesRatingData implements ISeriesRatingData {
    opinion!: string;
    rating!: number;
    seriesId!: string;
    userId!: number;

    constructor(data?: ISeriesRatingData) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.opinion = _data["opinion"];
            this.rating = _data["rating"];
            this.seriesId = _data["seriesId"];
            this.userId = _data["userId"];
        }
    }

    static fromJS(data: any): SeriesRatingData {
        data = typeof data === 'object' ? data : {};
        let result = new SeriesRatingData();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["opinion"] = this.opinion;
        data["rating"] = this.rating;
        data["seriesId"] = this.seriesId;
        data["userId"] = this.userId;
        return data; 
    }
}

export interface ISeriesRatingData {
    opinion: string;
    rating: number;
    seriesId: string;
    userId: number;
}

export class SeriesRatingInfo implements ISeriesRatingInfo {
    id!: number;
    opinion!: string;
    rating!: number;
    seriesId!: string;
    userId!: number;

    constructor(data?: ISeriesRatingInfo) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.id = _data["id"];
            this.opinion = _data["opinion"];
            this.rating = _data["rating"];
            this.seriesId = _data["seriesId"];
            this.userId = _data["userId"];
        }
    }

    static fromJS(data: any): SeriesRatingInfo {
        data = typeof data === 'object' ? data : {};
        let result = new SeriesRatingInfo();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["opinion"] = this.opinion;
        data["rating"] = this.rating;
        data["seriesId"] = this.seriesId;
        data["userId"] = this.userId;
        return data; 
    }
}

export interface ISeriesRatingInfo {
    id: number;
    opinion: string;
    rating: number;
    seriesId: string;
    userId: number;
}

export class ApiException extends Error {
    message: string;
    status: number;
    response: string;
    headers: { [key: string]: any; };
    result: any;

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }

    protected isApiException = true;

    static isApiException(obj: any): obj is ApiException {
        return obj.isApiException === true;
    }
}

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): any {
    if (result !== null && result !== undefined)
        throw result;
    else
        throw new ApiException(message, status, response, headers, null);
}