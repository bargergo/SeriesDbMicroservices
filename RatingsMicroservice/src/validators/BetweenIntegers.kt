package hu.bme.aut.ratings.validators

import com.papsign.ktor.openapigen.validators.ValidationAnnotation
import com.papsign.ktor.openapigen.validators.util.AValidator

@Retention(AnnotationRetention.RUNTIME)
@Target(AnnotationTarget.PROPERTY)
@ValidationAnnotation
annotation class BetweenIntegers(val lowerBound: Int, val upperBound: Int)

object BetweenIntegersValidator: AValidator<Int, BetweenIntegers>(Int::class, BetweenIntegers::class) {
    override fun validate(subject: Int?, annotation: BetweenIntegers): Int? {
        if (subject == null || subject < annotation.lowerBound || subject > annotation.upperBound)
            throw IllegalArgumentException("Invalid parameters")
        subject?.
        return subject
    }
}