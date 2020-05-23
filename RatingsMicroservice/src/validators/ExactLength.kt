package hu.bme.aut.ratings.validators

import com.papsign.ktor.openapigen.schema.processor.SchemaProcessorAnnotation
import com.papsign.ktor.openapigen.validation.ValidatorAnnotation

@Target(AnnotationTarget.TYPE, AnnotationTarget.PROPERTY)
@SchemaProcessorAnnotation(ExactLengthProcessor::class)
@ValidatorAnnotation(ExactLengthProcessor::class)
annotation class ExactLength(val value: Int, val message: String = "")