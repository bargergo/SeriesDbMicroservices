import { ErrorMessage, Field, Form, Formik } from "formik";
import React from "react";
import * as Yup from "yup";
import {
  SeriesClient,
  UpsertSeriesRequest,
} from "../typings/SeriesAndEpisodesClients";

const SeriesDetailForm = () => {
  const client: SeriesClient = new SeriesClient();
  return (
    <>
      <h1>Details of the Series</h1>
      <Formik
        initialValues={{
          title: "",
          description: "",
          firstAired: "",
        }}
        validationSchema={Yup.object({
          title: Yup.string()
            .max(15, "Must be 15 characters or less")
            .required("Required"),
          description: Yup.string()
            .max(255, "Must be 255 characters or less")
            .required("Required"),
          firstAired: Yup.string().required("Required"),
        })}
        onSubmit={async (values, { setSubmitting }) => {
          const request = new UpsertSeriesRequest({
            title: values.title,
            description: values.description,
            firstAired: new Date(values.firstAired),
          });
          try {
            const response = await client.createSeries(request);
            alert(JSON.stringify(response));
          } catch (err) {
            alert(err);
          }
        }}
      >
        {({ isSubmitting }) => (
          <Form>
            <label htmlFor="title">Title</label>
            <Field type="text" name="title" />
            <ErrorMessage name="title" component="div" />
            <label htmlFor="description">Description</label>
            <Field type="text" name="description" />
            <ErrorMessage name="description" component="div" />
            <label htmlFor="firstAired">First Aired</label>
            <Field type="text" name="firstAired" />
            <ErrorMessage name="firstAired" component="div" />
            <button type="submit" disabled={isSubmitting}>
              Submit
            </button>
          </Form>
        )}
      </Formik>
    </>
  );
};

export default SeriesDetailForm;
