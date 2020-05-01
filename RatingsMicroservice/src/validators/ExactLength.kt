package hu.bme.aut.ratings.validators

import com.papsign.ktor.openapigen.validators.ValidationAnnotation
import com.papsign.ktor.openapigen.validators.util.AValidator

@Retention(AnnotationRetention.RUNTIME)
@Target(AnnotationTarget.PROPERTY)
@ValidationAnnotation
annotation class ExactLength(val length: Int)

object LengthValidator: AValidator<String, ExactLength>(String::class, ExactLength::class) {
    override fun validate(subject: String?, annotation: ExactLength): String? {
        if (subject?.length ?: 0 != annotation.length)
            throw IllegalArgumentException("Invalid parameters")
        return subject
    }
}