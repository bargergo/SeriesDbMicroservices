import { ErrorMessage, Field, Form, Formik } from "formik";
import React from "react";
import * as Yup from "yup";
import {
  SeriesRatingsClient,
  SeriesRatingData,
} from "../typings/RatingsClients";

const SeriesRatingForm = (props: { seriesId: string }) => {
  const client: SeriesRatingsClient = new SeriesRatingsClient();
  return (
    <>
      <h1>Add rating</h1>
      <Formik
        initialValues={{
          opinion: "",
          rating: 0,
          userId: -1,
        }}
        validationSchema={Yup.object({
          opinion: Yup.string()
            .max(255, "Must be 255 characters or less")
            .required("Required"),
          rating: Yup.number()
            .min(1, "Must be at least 1")
            .max(10, "Must be less or equal to 10")
            .required("Required"),
          userId: Yup.number().required("Required"),
        })}
        onSubmit={async (values, { setSubmitting }) => {
          const request = new SeriesRatingData({
            opinion: values.opinion,
            rating: values.rating,
            seriesId: props.seriesId,
            userId: values.userId,
          });
          try {
            const response = await client.createSeriesRating(request);
            alert(JSON.stringify(response));
          } catch (err) {
            alert(err);
          }
        }}
      >
        {({ isSubmitting }) => (
          <Form>
            <label htmlFor="opinion">Opinion</label>
            <Field type="text" name="opinion" />
            <ErrorMessage name="opinion" component="div" />
            <label htmlFor="rating">Rating</label>
            <Field type="number" name="rating" />
            <ErrorMessage name="rating" component="div" />
            <label htmlFor="userId">User ID</label>
            <Field type="number" name="userId" />
            <ErrorMessage name="userId" component="div" />
            <button type="submit" disabled={isSubmitting}>
              Submit
            </button>
          </Form>
        )}
      </Formik>
    </>
  );
};

export default SeriesRatingForm;
